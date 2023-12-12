import { 
    Autocomplete, 
    AutocompleteRenderOptionState, 
    Popper, 
    TextField, 
    autocompleteClasses, 
    styled
} from "@mui/material";
import { 
    HTMLAttributes, 
    ReactNode, 
    SyntheticEvent, 
    useCallback
} from "react";
import { FC } from "react";
import { AirportResponse } from "./FlightsApiSlice";
import VirtualListboxComponent from "../../components/VirtualListBox/VirtualListbox";

const StyledPopper = styled(Popper)({
    [`& .${autocompleteClasses.listbox}`]: {
        boxSizing: 'border-box',
        '& ul': {
            padding: 0,
            margin: 0,
        },
    },
});

interface Props {
    label: string;
    value?: AirportResponse | null;
    airports: AirportResponse[];
    onChange: (newValue: AirportResponse) => void;
}

const AirportsAutocomplete: FC<Props> = ({ label, value, airports, onChange }) => {
    const renderOption = useCallback((
        props: HTMLAttributes<HTMLLIElement>,
        option: any,
        state: AutocompleteRenderOptionState): ReactNode | undefined =>
        [props, option, state.inputValue] as ReactNode,
        []);

    const formatOption = useCallback((option: AirportResponse) => option.name, []);

    const handleChange = useCallback((event: SyntheticEvent<Element, Event>, newValue: any | null) => {
        onChange(newValue);
    }, []);

    return (
        <Autocomplete
            sx={{ width: "200px", minWidth: "50px" }}
            value={value}
            disableListWrap
            PopperComponent={StyledPopper}
            ListboxComponent={VirtualListboxComponent}
            options={airports}
            renderInput={(params) => <TextField {...params} label={label} />}
            getOptionLabel={formatOption}
            renderOption={renderOption}
            onChange={handleChange}
        />
    );
}

export default AirportsAutocomplete;