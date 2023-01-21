export interface IUser {
    email: string;
    displayName: string;
    token: string;
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