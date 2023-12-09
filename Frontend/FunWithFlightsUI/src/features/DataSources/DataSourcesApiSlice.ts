import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { ListResponse } from '../../api/Common';

const URL = process.env['services__datasources-api__1'];

export interface DataSourceResponse {
    id: number;
    name: string;
    description: string;
    url: string;
}

export const dataSourcesApiSlice = createApi({
    reducerPath: 'dataSourcesApi',
    baseQuery: fetchBaseQuery({ baseUrl: URL }),
    endpoints: (builder) => ({
        getDataSources: builder.query<ListResponse<DataSourceResponse>, void>({
            query: () => `api/data-sources/all`
        })
    })
});

export const { 
    useGetDataSourcesQuery
} = dataSourcesApiSlice;