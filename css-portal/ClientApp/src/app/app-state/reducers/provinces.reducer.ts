
import { createReducer, on } from '@ngrx/store';
import { setProvinces, clearProvinces } from '@actions/provinces.actions';
import { Province } from '@models/province.model';

export interface ProvinceState {
    provinces: Province[] | null;
}

export const initialState = null;

const _provincesReducer = createReducer(initialState,
    on(setProvinces, (_state, action) => action.provinces),
    on(clearProvinces, _state => null)
);

export function provincesReducer(state, action) {
    return _provincesReducer(state, action);
}
