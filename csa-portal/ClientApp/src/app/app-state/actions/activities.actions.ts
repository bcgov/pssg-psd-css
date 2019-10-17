import { createAction, props } from '@ngrx/store';
import { Activity } from '@models/activity.model';

export const setActivities = createAction('SET_ACTIVITIES', props<{ activities: Activity[] }>());
export const clearActivities = createAction('CLEAR_ACTIVITIES');
