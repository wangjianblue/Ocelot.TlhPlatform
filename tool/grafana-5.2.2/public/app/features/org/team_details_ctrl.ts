import coreModule from 'app/core/core_module';

export default class TeamDetailsCtrl {
  team: Team;
  teamMembers: User[] = [];
  navModel: any;

  /** @ngInject **/
  constructor(private $scope, private backendSrv, private $routeParams, navModelSrv) {
    this.navModel = navModelSrv.getNav('cfg', 'teams', 0);
    this.userPicked = this.userPicked.bind(this);
    this.get = this.get.bind(this);
    this.get();
  }

  get() {
    if (this.$routeParams && this.$routeParams.id) {
      this.backendSrv.get(`/api/teams/${this.$routeParams.id}`).then(result => {
        this.team = result;
      });
      this.backendSrv.get(`/api/teams/${this.$routeParams.id}/members`).then(result => {
        this.teamMembers = result;
      });
    }
  }

  removeTeamMember(teamMember: TeamMember) {
    this.$scope.appEvent('confirm-modal', {
      title: 'Remove Member',
      text: 'Are you sure you want to remove ' + teamMember.login + ' from this group?',
      yesText: 'Remove',
      icon: 'fa-warning',
      onConfirm: () => {
        this.removeMemberConfirmed(teamMember);
      },
    });
  }

  removeMemberConfirmed(teamMember: TeamMember) {
    this.backendSrv.delete(`/api/teams/${this.$routeParams.id}/members/${teamMember.userId}`).then(this.get);
  }

  update() {
    if (!this.$scope.teamDetailsForm.$valid) {
      return;
    }

    this.backendSrv.put('/api/teams/' + this.team.id, {
      name: this.team.name,
      email: this.team.email,
    });
  }

  userPicked(user) {
    this.backendSrv.post(`/api/teams/${this.$routeParams.id}/members`, { userId: user.id }).then(() => {
      this.$scope.$broadcast('user-picker-reset');
      this.get();
    });
  }
}

export interface Team {
  id: number;
  name: string;
  email: string;
}

export interface User {
  id: number;
  name: string;
  login: string;
  email: string;
}

export interface TeamMember {
  userId: number;
  name: string;
  login: string;
}

coreModule.controller('TeamDetailsCtrl', TeamDetailsCtrl);
