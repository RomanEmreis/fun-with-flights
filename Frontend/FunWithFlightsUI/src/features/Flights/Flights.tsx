import { Button, Divider, Paper, Typography } from "@mui/material";
import { FC, SyntheticEvent, useCallback, useState } from "react";
import { FlightRoute, useGetAirportsQuery, useLazyFindRoutesQuery } from "./FlightsApiSlice";
import AirportsAutocomplete from "./AirportsAutocomplete";
import { DatePicker } from "../../components/DatePicker/DatePicker";
import dayjs from "dayjs";

const Flights: FC = () => {
    const [source, setSource] = useState<string>('');
    const [destination, setDestination] = useState<string>('');

    const [dateOfFlight, setDateOfFlight] = useState<Date | null>(null);
    const [dateOfReturn, setDateOfReturn] = useState<Date | null>(null);

    const [routes, setRoutes] = useState<FlightRoute[]>([]);

    const { data: airports } = useGetAirportsQuery();
    const [searchTrigger,] = useLazyFindRoutesQuery();

    const handleSourceChange = useCallback((newValue: string) => {
        setSource(newValue);
    }, []);

    const handleDestinationChange = useCallback((newValue: string) => {
        setDestination(newValue);
    }, []);

    const handleDateOfFlightChange = useCallback((newDate: Date | null, event: SyntheticEvent<any, Event> | undefined) => {
        setDateOfFlight(newDate);
    }, []);

    const handleDateOfReturnChange = useCallback((newDate: Date | null, event: SyntheticEvent<any, Event> | undefined) => {
        setDateOfReturn(newDate);
    }, []);

    const handleOnClick = useCallback(async () => {
        const { data, isSuccess } = await searchTrigger({
            sourceAirport: source,
            destinationAirport: destination,
            dateOfFlight: dayjs(dateOfFlight!).format('YYYY-MM-DD'),
            dateOfReturn: dayjs(dateOfReturn!).format('YYYY-MM-DD')
        });

        if (isSuccess) {
            setRoutes(data.results);
        }
    }, [source, destination, dateOfFlight, dateOfReturn]);

    return (
        <>
            <Paper
                component="form"
                elevation={4}
                sx={{ display: 'flex', alignItems: 'center', margin: "100px 200px", padding: "15px 30px" }}
            >
                <AirportsAutocomplete
                    label="From"
                    airports={airports?.availableSourceAirports ?? []}
                    onChange={handleSourceChange}
                />
                <Divider sx={{ height: 28, m: 0.5 }} orientation="vertical" />
                <AirportsAutocomplete
                    label="To"
                    airports={airports?.availableDestinationAirports ?? []}
                    onChange={handleDestinationChange}
                />
                <Divider sx={{ height: 28, m: 0.5 }} orientation="vertical" />
                <DatePicker
                    label="Departure"
                    onChange={handleDateOfFlightChange} value={dateOfFlight} />
                <Divider sx={{ height: 28, m: 0.5 }} orientation="vertical" />
                <DatePicker
                    label="Return"
                    onChange={handleDateOfReturnChange} value={dateOfReturn} />
                <Divider sx={{ height: 28, m: 0.5 }} orientation="vertical" />
                <Button
                    sx={{ height: "56px", width: "100px" }}
                    variant="outlined"
                    onClick={handleOnClick}>
                    Search
                </Button>
            </Paper>
            {routes.map((route) => (
                <Paper
                    component="form"
                    elevation={2}
                    sx={{ display: 'flex', alignItems: 'center', margin: "10px 200px", padding: "15px 30px" }}>
                    <Typography textAlign="center" sx={{ m: 2, fontSize: 20 }}>{route.sourceAirport}</Typography>
                    <Typography textAlign="center" sx={{ m: 1 }}>&#10230;</Typography>
                    <Typography textAlign="center" sx={{ m: 2, fontSize: 20 }}>{route.destinationAirport}</Typography>
                    <Divider sx={{ height: 28, m: 0.5 }} orientation="vertical" />
                    <Typography textAlign="center" sx={{ m: 1, fontSize: 20, fontWeight: 700 }}>Airline</Typography>
                    <Typography textAlign="center" sx={{ m: 2, fontSize: 20 }}>{route.airline}</Typography>
                    <Divider sx={{ height: 28, m: 0.5 }} orientation="vertical" />
                    <Typography textAlign="center" sx={{ m: 1, fontSize: 20, fontWeight: 700 }}>Equipment</Typography>
                    <Typography textAlign="center" sx={{ m: 2, fontSize: 20 }}>{route.equipment}</Typography>
                </Paper>
            ))}
        </>
    );
}

export default Flights;