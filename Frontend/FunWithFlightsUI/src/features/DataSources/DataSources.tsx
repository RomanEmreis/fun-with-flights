import {
    Container,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow
} from "@mui/material";
import { FC } from "react";
import { useGetDataSourcesQuery } from "./DataSourcesApiSlice";

const DataSources: FC = () => {
    const { data } = useGetDataSourcesQuery();

    return (
        <Container maxWidth="xl">
            <TableContainer component={Paper} elevation={4} sx={{ 
                width: 700, 
                display: { xs: 'none', md: 'flex' },
                m: 10
            }}>
                <Table aria-label="simple table">
                    <TableHead>
                        <TableRow>
                            <TableCell>Name</TableCell>
                            <TableCell align="right">Description</TableCell>
                            <TableCell align="right">URL</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {data?.results.map((row) => (
                            <TableRow
                                key={row.id}
                                sx={{ '&:last-child td, &:last-child th': { border: 0 } }}>
                                <TableCell component="th" scope="row">
                                    {row.name}
                                </TableCell>
                                <TableCell align="right">{row.description}</TableCell>
                                <TableCell align="right">
                                    <a href={row.url} target="_blank">Link</a>
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
            <TableContainer component={Paper} elevation={4} sx={{ 
                minWidth: 150, 
                flexGrow: 1, 
                display: { xs: 'flex', md: 'none' }, 
                m: "15px 5px" }}>
                <Table aria-label="simple table">
                    <TableHead>
                        <TableRow>
                            <TableCell>Name</TableCell>
                            <TableCell align="right">Description</TableCell>
                            <TableCell align="right">URL</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {data?.results.map((row) => (
                            <TableRow
                                key={row.id}
                                sx={{ '&:last-child td, &:last-child th': { border: 0 } }}>
                                <TableCell component="th" scope="row">
                                    {row.name}
                                </TableCell>
                                <TableCell align="right">{row.description}</TableCell>
                                <TableCell align="right">
                                    <a href={row.url} target="_blank">Link</a>
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </Container>
    );
}

export default DataSources;