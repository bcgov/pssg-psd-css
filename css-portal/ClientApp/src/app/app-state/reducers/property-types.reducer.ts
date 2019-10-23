
import { createReducer, on } from '@ngrx/store';
import { setPropertyTypes, clearPropertyTypes } from '@actions/property-types.actions';
import { PropertyType } from '@models/property-type.model';

export interface PropertyTypeState {
    propertyTypes: PropertyType[] | null;
}

export const initialState = null;

const _propertyTypesReducer = createReducer(initialState,
    on(setPropertyTypes, (_state, action) => action.propertyTypes),
    on(clearPropertyTypes, _state => null)
);

export function propertyTypesReducer(state, action) {
    return _propertyTypesReducer(state, action);
}
