export const API_ENDPOINTS = {
  //User
  getAll: 'GetAllUser',
  getById: 'GetUserById',
  getUserByUsernameOrEmail: 'GetUserByUsernameOrEmail',
  deleteUser: 'SofDeleteUser',

  //Auth
  login: 'Login',
  logout: 'Logout/logout',
  register: 'Register',
  resetPassword: 'ResetPassword',
  forgotPassword: 'ForgotPassword',
  refreshToken: 'RefreshToken/refresh',
  confirmEmail: 'ConfirmEmail',
  twoFactorAuth: 'TwoFactorAuthentication',
  enable2FactorAuth: 'EnableTwoFactorAuth',
  disable2FactorAuth: 'DisableTwoFactorAuth',
}
