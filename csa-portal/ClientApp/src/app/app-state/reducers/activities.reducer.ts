
import { createReducer, on } from '@ngrx/store';
import { setActivities, clearActivities } from '@actions/activities.actions';
import { Activity } from '@models/activity.model';

export interface ActivityState {
    activities: Activity[] | null;
}

export const initialState = null;

const _activitiesReducer = createReducer(initialState,
    on(setActivities, (_state, action) => action.activities),
    on(clearActivities, _state => null)
);

export function activitiesReducer(state, action) {
    return _activitiesReducer(state, action);
}
