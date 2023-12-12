import { Container } from "@mui/material";
import { FC } from "react";
import { useAppSelector } from "../../app/hooks";
import { selectSearchState } from "./FlightsSlice";
import FlightRouteCard from "./FlightRouteCard";
import SearchBar from "./SearchBar";

const Flights: FC = () => {
    const { 
        searchResult
    } = useAppSelector(selectSearchState);

    return (
        <Container maxWidth="xl">
            <SearchBar />
            {searchResult.map((route) => (<FlightRouteCard route={route} />))}
        </Container>
    );
}

export default Flights;