import { PayloadAction, createSelector, createSlice } from "@reduxjs/toolkit";
import { RootState } from "../../app/store";
import { AirportResponse, FlightRoute } from "./FlightsApiSlice";

export interface FlightSearchState {
    sourceAirport: AirportResponse | null;
    destinationAirport: AirportResponse | null;
    dateOfFlight: Date | null;
    dateOfReturn: Date | null;
    searchResult: FlightRoute[];
};

const initialState: FlightSearchState = {
    sourceAirport: null,
    destinationAirport: null,
    dateOfFlight: null,
    dateOfReturn: null,
    searchResult: []
};

const slice = createSlice({
	name: 'siteSearch',
	initialState: initialState,
	reducers: {
        setSearchState: (state, action: PayloadAction<Partial<FlightSearchState>>) => {
            return { ...state, ...action.payload };
        }
	},
});

const getSliceRoot = (rootState: RootState) => rootState.flightSearchSlice;

export default slice.reducer;
export const selectSearchState = createSelector(getSliceRoot, (state: FlightSearchState) => state);
export const { setSearchState } = slice.actions;