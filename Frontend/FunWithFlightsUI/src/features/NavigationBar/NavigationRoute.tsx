import { Button, MenuItem, Typography } from "@mui/material";
import { FC, useCallback } from "react";
import { useNavigate } from "react-router-dom";

export interface RouteInfo {
    title: string;
    path: string;
}

interface Props {
    route: RouteInfo;
    onClick?: () => void;
}

export const MenuItemRoute: FC<Props> = ({ route, onClick }) => {
    const navigate = useNavigate();
    const handleClick = useCallback(() => {
        onClick?.();
        navigate(route.path);
    }, [route.path, onClick]);

    return (
        <MenuItem key={route.title} onClick={handleClick}>
            <Typography textAlign="center">{route.title}</Typography>
        </MenuItem>
    );
}

export const ButtonRoute: FC<Props> = ({ route, onClick }) => {
    const navigate = useNavigate();
    const handleClick = useCallback(() => {
        onClick?.();
        navigate(route.path);
    }, [route.path, onClick]);

    return (
        <Button
            key={route.title}
            onClick={handleClick}
            sx={{ my: 2, color: 'white', display: 'block' }}
        >
            {route.title}
        </Button>
    );
}
