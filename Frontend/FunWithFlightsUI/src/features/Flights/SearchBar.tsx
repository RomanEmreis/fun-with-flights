import { 
    Paper, 
    Divider, 
    Button, 
    Card, 
    CardContent 
} from "@mui/material";
import { 
    FC, 
    SyntheticEvent, 
    useCallback 
} from "react";
import { 
    selectSearchState, 
    setSearchState 
} from "./FlightsSlice";
import { 
    useAppSelector, 
    useAppDispatch 
} from "../../app/hooks";
import { 
    useGetAirportsQuery, 
    useLazyFindRoutesQuery, 
    AirportResponse 
} from "./FlightsApiSlice";
import { DatePicker } from "../../components/DatePicker/DatePicker";
import dayjs from "dayjs";
import AirportsAutocomplete from "./AirportsAutocomplete";

interface Props {

}

const SearchBar: FC<Props> = () => {
    const { 
        sourceAirport,
        destinationAirport,
        dateOfFlight, 
        dateOfReturn
    } = useAppSelector(selectSearchState);

    const dispatch = useAppDispatch();

    const { data: airports } = useGetAirportsQuery();
    const [searchTrigger,] = useLazyFindRoutesQuery();

    const handleSourceChange = useCallback((newValue: AirportResponse) => {
        dispatch(setSearchState({ sourceAirport: newValue }));
    }, []);

    const handleDestinationChange = useCallback((newValue: AirportResponse) => {
        dispatch(setSearchState({ destinationAirport: newValue }));
    }, []);

    const handleDateOfFlightChange = useCallback((newDate: Date | null, event: SyntheticEvent<any, Event> | undefined) => {
        dispatch(setSearchState({ dateOfFlight: newDate }));
    }, []);

    const handleDateOfReturnChange = useCallback((newDate: Date | null, event: SyntheticEvent<any, Event> | undefined) => {
        dispatch(setSearchState({ dateOfReturn: newDate }));
    }, []);

    const handleOnClick = useCallback(async () => {
        if (sourceAirport === null || destinationAirport === null) {
            return;
        }

        const { data, isSuccess } = await searchTrigger({
            sourceAirport: sourceAirport.name,
            destinationAirport: destinationAirport.name,
            dateOfFlight: dayjs(dateOfFlight!).format('YYYY-MM-DD'),
            dateOfReturn: dayjs(dateOfReturn!).format('YYYY-MM-DD')
        });

        if (isSuccess) {
            dispatch(setSearchState({ searchResult: data.results }));
        }
    }, [sourceAirport, destinationAirport, dateOfFlight, dateOfReturn]);
    
    return (
        <>
            <Paper
                component="form"
                elevation={4}
                sx={{
                    display: { xs: 'none', md: 'flex' },
                    alignItems: 'center',
                    margin: "100px 200px",
                    padding: "15px 30px"
                }}>
                <AirportsAutocomplete
                    label="From"
                    value={sourceAirport}
                    airports={airports?.availableSourceAirports ?? []}
                    onChange={handleSourceChange}
                />
                <Divider sx={{ height: 28, m: 0.5 }} orientation="vertical" />
                <AirportsAutocomplete
                    label="To"
                    value={destinationAirport}
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
            <Card
                component="form"
                elevation={4}
                sx={{
                    display: { xs: 'flex', md: 'none' },
                    flexDirection: "column",
                    alignItems: 'center',
                    justifyContent: 'stretch',
                    margin: "10px 10px",
                    padding: "10px 20px"
                }}>
                <CardContent>
                    <AirportsAutocomplete
                        label="From"
                        value={sourceAirport}
                        airports={airports?.availableSourceAirports ?? []}
                        onChange={handleSourceChange}
                    />
                    <Divider sx={{ m: 0.5 }} orientation="horizontal" />
                    <AirportsAutocomplete
                        label="To"
                        airports={airports?.availableDestinationAirports ?? []}
                        onChange={handleDestinationChange}
                    />
                    <Divider sx={{ m: 0.5 }} orientation="horizontal" />
                    <DatePicker
                        label="Departure"
                        onChange={handleDateOfFlightChange} value={dateOfFlight} />
                    <Divider sx={{ m: 0.5 }} orientation="horizontal" />
                    <DatePicker
                        label="Return"
                        onChange={handleDateOfReturnChange} value={dateOfReturn} />
                    <Divider sx={{ m: 0.5 }} orientation="horizontal" />
                    <Button
                        sx={{ height: "56px", width: "100px" }}
                        variant="outlined"
                        onClick={handleOnClick}>
                        Search
                    </Button>
                </CardContent>
            </Card>
        </>
    );
};

export default SearchBar;