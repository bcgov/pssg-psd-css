import { createAction, props } from '@ngrx/store';
import { PropertyType } from '@models/property-type.model';

export const setPropertyTypes = createAction('SET_PROPERTY_TYPES', props<{ propertyTypes: PropertyType[] }>());
export const clearPropertyTypes = createAction('CLEAR_PROPERTY_TYPES');
