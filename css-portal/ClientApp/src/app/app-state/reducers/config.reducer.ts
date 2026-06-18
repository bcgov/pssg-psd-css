import { createReducer, on } from '@ngrx/store';
import { setConfig } from '@actions/config.actions';
import { ServerConfig } from '@services/config-data.service';

export const initialState: ServerConfig | null = null;

const _configReducer = createReducer<ServerConfig | null>(
  initialState,
  on(setConfig, (_state, action) => action.config)
);

export function configReducer(state: ServerConfig | null = initialState, action: any) {
  return _configReducer(state, action);
}
