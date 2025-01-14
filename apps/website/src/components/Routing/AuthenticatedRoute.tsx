import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "../Authentication/AuthenticationProvider";


export default function AuthenticatedRoute() {
    const { isAuthenticated } = useAuth();

    return isAuthenticated ? <Outlet/> : <Navigate to='/login'/>
}