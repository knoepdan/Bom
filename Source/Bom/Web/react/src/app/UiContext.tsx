import * as React from 'react';

export enum NotificationType {
    Success = 'S',
    Error = 'E',
    Warning = 'W',
    Info = 'I',
}

export interface INotificationMessage {
    text: string;
    type: NotificationType;
}

export interface IUiContext {
    // notifications
    showNotification(msg: INotificationMessage): void;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    showError(e: unknown): void;
}
export const UiContext = React.createContext<IUiContext>(undefined as unknown as IUiContext);

export function useUiContext(): IUiContext {
    const context: IUiContext = React.useContext(UiContext);
    return context;
}

export default useUiContext;
