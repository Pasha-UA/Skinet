export interface IUser {
    email: string;
    displayName: string;
    token: string;
    emailConfirmationRequired?: boolean;
    rememberMe: boolean;
}
