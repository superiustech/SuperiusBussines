// ProtectedRoute.js
import { Navigate, useLocation } from 'react-router-dom';
import { useAuth } from './AuthContext';

const ProtectedRoute = ({ children }) => {
    const { isAuthenticated } = useAuth();
    const location = useLocation();

    if (!isAuthenticated()) { return <Navigate to="/administrador/login" state={{ from: location }} replace />; }

    return children;
};
export default ProtectedRoute;