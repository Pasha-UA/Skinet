export interface IUser {
    email: string;
    displayName: string;
    token: string;
    phoneNumber: string;
    emailConfirmationRequired?: boolean;
    rememberMe: boolean;
    accountLocked: boolean;
    roles: IRole[];
}

export interface IRole
{
    id: string;
    name: string;    
}