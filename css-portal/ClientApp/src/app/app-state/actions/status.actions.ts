import { createAction, props } from '@ngrx/store';
import { Status } from '@models/status.model';

export const setStatus = createAction('SET_STATUS', props<{ status: Status }>());
