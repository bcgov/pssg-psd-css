import { createAction, props } from "@ngrx/store";
import { ServerConfig } from "@services/config-data.service";

export const setConfig = createAction(
  "SET_CONFIG",
  props<{ config: ServerConfig | null }>(),
);
