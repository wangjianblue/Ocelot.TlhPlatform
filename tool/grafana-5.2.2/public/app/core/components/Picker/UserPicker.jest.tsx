﻿import React from 'react';
import renderer from 'react-test-renderer';
import UserPicker from './UserPicker';

const model = {
  backendSrv: {
    get: () => {
      return new Promise((resolve, reject) => {});
    },
  },
  handlePicked: () => {},
};

describe('UserPicker', () => {
  it('renders correctly', () => {
    const tree = renderer.create(<UserPicker {...model} />).toJSON();
    expect(tree).toMatchSnapshot();
  });
});
