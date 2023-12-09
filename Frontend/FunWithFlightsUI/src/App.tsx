import NavigationBar from "./features/NavigationBar/NavigationBar"
import { Navigate, Route, Routes } from "react-router-dom"
import DataSources from "./features/DataSources/DataSources"
import Flights from "./features/Flights/Flights"
import Bookings from "./features/Bookings/Bookings"

import "./App.scss"

function App() {
    return (
        <div className="App">
            <header className="App-header">
                <NavigationBar />
            </header>
            <div className="app-content">
                <Routes>
                    <Route path='/' element={<Navigate replace to='/flights' />} />
                    <Route path='/flights' element={<Flights />}></Route>
                    <Route path='/bookings' element={<Bookings />}></Route>
                    <Route path='/data-sources' element={<DataSources />}></Route>
                </Routes>
            </div>
        </div>
    )
}

export default App
