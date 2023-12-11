import { Autocomplete, AutocompleteRenderOptionState, /*  */Popper, TextField, Typography, autocompleteClasses, styled, useMediaQuery, useTheme } from "@mui/material";
import React, { HTMLAttributes, ReactNode, SyntheticEvent, useCallback, useMemo } from "react";
import { FC } from "react";
import { VariableSizeList, ListChildComponentProps } from 'react-window';
import { AirportResponse } from "./FlightsApiSlice";

const LISTBOX_PADDING = 8; // px

const renderRow = (props: ListChildComponentProps) => {
    const { data, index, style } = props;
    const dataSet = data[index];
    const inlineStyle = {
        ...style,
        top: (style.top as number) + LISTBOX_PADDING,
    };

    return (
        <Typography component="li" {...dataSet[0]} noWrap style={inlineStyle}>
            {dataSet[1].name}
        </Typography>
    );
}

const OuterElementContext = React.createContext({});

const OuterElementType = React.forwardRef<HTMLDivElement>((props, ref) => {
    const outerProps = React.useContext(OuterElementContext);
    return <div ref={ref} {...props} {...outerProps} />;
});

const useResetCache = (data: any) => {
    const ref = React.useRef<VariableSizeList>(null);
    React.useEffect(() => {
        if (ref.current != null) {
            ref.current.resetAfterIndex(0, true);
        }
    }, [data]);
    return ref;
}

// Adapter for react-window
const ListboxComponent = React.forwardRef<
    HTMLDivElement,
    React.HTMLAttributes<HTMLElement>
>(function ListboxComponent(props, ref) {
    const { children, ...other } = props;
    const itemData = useMemo(() => {
        const itemData: React.ReactElement[] = [];
        (children as React.ReactElement[]).forEach(
          (item: React.ReactElement & { children?: React.ReactElement[] }) => {
            itemData.push(item);
            itemData.push(...(item.children || []));
          },
        );
        return itemData;
      }, [children]);

    const theme = useTheme();
    const smUp = useMediaQuery(theme.breakpoints.up('sm'), {
        noSsr: true,
    });
    const itemCount = itemData.length;
    const itemSize = smUp ? 36 : 48;

    const getChildSize = (child: React.ReactElement) => {
        if (child.hasOwnProperty('group')) {
            return 48;
        }
        return itemSize;
    };

    const getHeight = () => {
        if (itemCount > 8) {
            return 8 * itemSize;
        }
        return itemData.map(getChildSize).reduce((a, b) => a + b, 0);
    };

    const gridRef = useResetCache(itemCount);

    return (
        <div ref={ref}>
            <OuterElementContext.Provider value={other}>
                <VariableSizeList
                    itemData={itemData}
                    height={getHeight() + 2 * LISTBOX_PADDING}
                    width="100%"
                    ref={gridRef}
                    outerElementType={OuterElementType}
                    innerElementType="ul"
                    itemSize={(index) => getChildSize(itemData[index])}
                    overscanCount={5}
                    itemCount={itemCount}
                >
                    {renderRow}
                </VariableSizeList>
            </OuterElementContext.Provider>
        </div>
    );
});

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
    airports: AirportResponse[];
    onChange: (newValue: string) => void;
}

const AirportsAutocomplete: FC<Props> = ({ label, airports, onChange }) => {
    const renderOption = useCallback((
        props: HTMLAttributes<HTMLLIElement>,
        option: any,
        state: AutocompleteRenderOptionState): ReactNode | undefined =>
        [props, option, state.inputValue] as ReactNode,
        []);

    const formatOption = useCallback((option: AirportResponse) => option.name, []);

    const handleChange = useCallback((event: SyntheticEvent<Element, Event>, newValue: any | null) => {
        onChange(newValue.name);
    }, []);

    return (
        <Autocomplete
            sx={{ width: "200px", minWidth: "50px" }}
            disableListWrap
            PopperComponent={StyledPopper}
            ListboxComponent={ListboxComponent}
            options={airports}
            renderInput={(params) => <TextField {...params} label={label} />}
            getOptionLabel={formatOption}
            renderOption={renderOption}
            onChange={handleChange}
        />
    );
}

export default AirportsAutocomplete;