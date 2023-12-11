import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { ListResponse } from '../../api/Common';

const URL = process.env['services__aggregator-api__1'];

export interface AirportsResponse {
    availableSourceAirports: AirportResponse[];
    availableDestinationAirports: AirportResponse[];
}

export interface AirportResponse {
    name: string;
}

export interface FindRoutesRequest {
    sourceAirport: string;
    destinationAirport: string;
    dateOfFlight: string;
    dateOfReturn?: string | null;
}

export interface FlightRoute {
    airline: string;
    sourceAirport: string;
    destinationAirport: string;
    codeShare: string;
    stops: number;
    equipment: string;
}

export const aggregatorApiSlice = createApi({
    reducerPath: 'aggregatorApi',
    baseQuery: fetchBaseQuery({ baseUrl: URL }),
    endpoints: (builder) => ({
        getAirports: builder.query<AirportsResponse, void>({
            query: () => `api/flight-routes/airports`
        }),
        findRoutes: builder.query<ListResponse<FlightRoute>, FindRoutesRequest>({
            query: (request) => ({
                url: `api/flight-routes/find`,
                body: request,
                method: 'PUT'
            })
        })
    })
});

export const {
    useGetAirportsQuery,
    useLazyFindRoutesQuery
} = aggregatorApiSlice;