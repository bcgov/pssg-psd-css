
import { createReducer, on } from '@ngrx/store';
import { setStatus } from '@actions/status.actions';
import { Status } from '@models/status.model';

export interface StatusState {
    status: Status[] | null;
}

export const initialState = null;

const _statusReducer = createReducer(initialState,
    on(setStatus, (_state, action) => action.status)
);

export function statusReducer(state, action) {
    return _statusReducer(state, action);
}
