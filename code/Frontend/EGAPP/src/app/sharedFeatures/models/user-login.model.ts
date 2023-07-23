export interface UserLoggedIn {
  access_token: string;
  email: string;
  expires: any;
  expires_in: any;
  firstName: string;
  lastName: string;
  name: string;
  id: any;
  issued: any;
  mobile: string;
  phone: string;
  permissions: number[];
  token_type: string;
  userName: string;
  isFirstTimeLogin: boolean;
  organizationId: number;
  organizationName: string;
  organizationlastName:string;
  organizationfirstName:string;
  organizationImage: string;
  profileImage: string;
  roleName: string;
}
