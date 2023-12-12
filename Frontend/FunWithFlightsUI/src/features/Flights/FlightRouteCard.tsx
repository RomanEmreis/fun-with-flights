import { 
    Card, 
    Typography, 
    Divider, 
    CardActions, 
    Button, 
    Box, 
    CardContent 
} from "@mui/material";
import { FC } from "react";
import { FlightRoute } from "./FlightsApiSlice";

interface Props {
    route: FlightRoute;
}

const FlightRouteCard: FC<Props> = ({ route }) => {
    return (
        <>
            <Card
                component="form"
                elevation={2}
                sx={{ display: { xs: 'none', md: 'flex' }, alignItems: 'center', justifyContent: "space-between", margin: "10px 200px", padding: "15px 30px" }}>
                <Typography textAlign="center" sx={{ m: 2, fontSize: 20 }}>{route.sourceAirport}</Typography>
                <Typography textAlign="center" sx={{ m: 1 }}>&#10230;</Typography>
                <Typography textAlign="center" sx={{ m: 2, fontSize: 20 }}>{route.destinationAirport}</Typography>
                <Divider sx={{ height: 28, m: 0.5 }} orientation="vertical" />
                <Typography textAlign="center" sx={{ m: 1, fontSize: 20, fontWeight: 700 }}>Airline</Typography>
                <Typography textAlign="center" sx={{ m: 2, fontSize: 20 }}>{route.airline}</Typography>
                <Divider sx={{ height: 28, m: 0.5 }} orientation="vertical" />
                <Typography textAlign="center" sx={{ m: 1, fontSize: 20, fontWeight: 700 }}>Equipment</Typography>
                <Typography textAlign="center" sx={{ m: 2, fontSize: 20 }}>{route.equipment}</Typography>
                <CardActions>
                    <Button size="small">Book</Button>
                </CardActions>
            </Card>
            <Card
                component="form"
                elevation={2}
                sx={{ display: { xs: 'flex', md: 'none' }, flexDirection: "row", alignItems: 'center', margin: "10px 10px", padding: "10px 20px" }}>
                <Box sx={{ display: 'flex', flexDirection: 'column', justifyContent: "space-between" }}>
                    <CardContent sx={{ flex: '1 0 auto', justifyContent: "space-between" }}>
                        <Box sx={{ display: 'flex', flexDirection: 'row', justifyContent: "space-between", alignItems: 'center' }}>
                            <Typography textAlign="center" sx={{ m: 2, fontSize: 20 }}>{route.sourceAirport}</Typography>
                            <Typography textAlign="center" sx={{ m: 1 }}>&#10230;</Typography>
                            <Typography textAlign="center" sx={{ m: 2, fontSize: 20 }}>{route.destinationAirport}</Typography>
                        </Box>
                        <Divider sx={{ m: 0.5 }} orientation="horizontal" />
                        <Box sx={{ display: 'flex', flexDirection: 'row', justifyContent: "space-between", alignItems: 'center' }}>
                            <Typography textAlign="center" sx={{ m: 1, fontSize: 20, fontWeight: 700 }}>Airline</Typography>
                            <Typography textAlign="center" sx={{ m: 2, fontSize: 20 }}>{route.airline}</Typography>
                        </Box>
                        <Divider sx={{ m: 0.5 }} orientation="horizontal" />
                        <Box sx={{ display: 'flex', flexDirection: 'row', justifyContent: "space-between", alignItems: 'center' }}>
                            <Typography textAlign="center" sx={{ m: 1, fontSize: 20, fontWeight: 700 }}>Equipment</Typography>
                            <Typography textAlign="center" sx={{ m: 2, fontSize: 20 }}>{route.equipment}</Typography>
                        </Box>
                    </CardContent>
                    <CardActions disableSpacing>
                        <Button size="small">Book</Button>
                    </CardActions>
                </Box>
            </Card>
        </>
    );
};

export default FlightRouteCard;