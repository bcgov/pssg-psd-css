import { createAction, props } from '@ngrx/store';
import { Province } from '@models/province.model';

export const setProvinces = createAction('SET_PROVINCES', props<{ provinces: Province[] }>());
export const clearProvinces = createAction('CLEAR_PROVINCES');
