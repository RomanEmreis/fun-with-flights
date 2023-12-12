import { 
    InputAdornment, 
    Popover, 
    SxProps, 
    TextField
} from "@mui/material";
import { 
    FC, 
    SyntheticEvent, 
    useCallback, 
    useRef, 
    useState
} from "react";
import ReactDatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';
import CalendarMonthIcon from '@mui/icons-material/CalendarMonth';

export interface DatePickerParameters {
    sx?: SxProps;
    label?: string;
    value?: Date | null;
    onChange: (date: Date | null, event: SyntheticEvent<any, Event> | undefined) => void;
    isInErrorState?: boolean;
    helperMessage?: any;
};

export const DatePicker: FC<DatePickerParameters> = ({
    sx,
    label = 'Date',
    value,
    onChange,
    isInErrorState = false,
    helperMessage
}) => {
    const textFieldRef = useRef<HTMLDivElement | null>(null);
    const [isModalOpen, setIsModalOpen] = useState<boolean>(false);

    const inputDisableKeyboardHandler = useCallback(
        (event: React.KeyboardEvent<HTMLDivElement>) => event.preventDefault(),
    []);

    const inputPreventDeletesHandler = useCallback(
        (event: React.KeyboardEvent<HTMLDivElement>) =>
            (event.key === 'Delete' || event.key === 'Backspace') && event.preventDefault(),
        []);

    const inputKeyUpHandler = useCallback(
        (event: React.KeyboardEvent<HTMLDivElement>) => {
            if ((event.target as HTMLInputElement).type !== 'button' &&
                (event.key === 'Enter' || event.key === ' ')) {
                setIsModalOpen(!isModalOpen)
            }
        }, [isModalOpen]);

    const modalKeyUpHandler = useCallback(
        (event: React.KeyboardEvent<HTMLDivElement>) => {
            if (event.key === 'Escape') {
                setIsModalOpen(false)
            }
        }, []);

    const toggleModal = useCallback(() => {
        setIsModalOpen(!isModalOpen)
    }, [isModalOpen]);

    return (
        <>
            <TextField
                color='primary'
                ref={textFieldRef}
                label={value ? null : label}
                sx={sx}
                InputProps={{
                    endAdornment: (
                        <InputAdornment position="start" sx={{ cursor: 'pointer' }}>
                            <CalendarMonthIcon />
                        </InputAdornment>
                    ),
                }}
                onBeforeInput={inputDisableKeyboardHandler}
                onKeyDown={inputPreventDeletesHandler}
                onKeyUp={inputKeyUpHandler}
                onClick={toggleModal}
                error={isInErrorState}
                helperText={helperMessage}
                value={value?.toDateString()} />
            <Popover
                open={isModalOpen}
                anchorEl={textFieldRef.current}
                anchorOrigin={{
                    vertical: "bottom",
                    horizontal: "left"
                }}
                disableEscapeKeyDown={false}
                onKeyUp={modalKeyUpHandler}>
                <ReactDatePicker
                    inline={true}
                    onClickOutside={toggleModal}
                    shouldCloseOnSelect={true}
                    showPopperArrow={false}
                    preventOpenOnFocus={true}
                    selected={value}
                    onChange={onChange}
                    onKeyDown={inputKeyUpHandler}
                    onChangeRaw={(event: any) => event.preventDefault()}
                    open={isModalOpen}
                    onCalendarClose={toggleModal}
                    onSelect={toggleModal} />
            </Popover>
        </>
    );
};
