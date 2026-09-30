import { Routes, Route } from 'react-router-dom';
// Import the pages
import SummorizeTextPage from '../components/SummorizeTextCardContent';
import HomePage from '../pages/HomePage';

export default function AppRouter() { 
	return ( 
		<Routes> 
			// Looks at the URL and shows the right page-component. 
            <Route path="/" element ={<HomePage/>}/>
			<Route path="/ai/summarizeText" element={<SummorizeTextPage/>} /> 
			<Route path="*" element={<h2>404 - Page can not be found</h2>} /> 
		</Routes> );}