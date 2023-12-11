import { configureStore, ThunkAction, Action, combineReducers } from "@reduxjs/toolkit"
import { dataSourcesApiSlice } from "../features/DataSources/DataSourcesApiSlice"
import { aggregatorApiSlice } from "../features/Flights/FlightsApiSlice"

const rootReducer = combineReducers({
  [dataSourcesApiSlice.reducerPath]: dataSourcesApiSlice.reducer,
  [aggregatorApiSlice.reducerPath]: aggregatorApiSlice.reducer
});

const middlewares = [
  dataSourcesApiSlice.middleware,
  aggregatorApiSlice.middleware
];

export const store = configureStore({
  reducer: rootReducer,
  middleware: (getDefaultMiddleware) =>
      getDefaultMiddleware().concat(...middlewares)
});

export type AppDispatch = typeof store.dispatch
export type RootState = ReturnType<typeof store.getState>
export type AppThunk<ReturnType = void> = ThunkAction<
  ReturnType,
  RootState,
  unknown,
  Action<string>
>
