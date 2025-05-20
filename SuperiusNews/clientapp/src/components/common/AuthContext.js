import { createContext, useContext, useState, useCallback } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';

const decodeToken = (token) => {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));
    return JSON.parse(jsonPayload);
};

const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
    const [user, setUser] = useState(null);

    const navigate = useNavigate();

    const isAuthenticated = useCallback(() => {
        const token = localStorage.getItem('authToken');
        if (!token) return false;

        const decodedToken = decodeToken(token);
        const currentTime = Date.now() / 1000; 
        if (decodedToken.exp < currentTime) {
            logout(); 
            return false;
        }

        return true;
    }, []);

    const login = useCallback((token) => {
        const decodedToken = decodeToken(token);
        localStorage.setItem('authToken', token);
        setUser(decodedToken.unique_name);
    }, []);

    const logout = useCallback(() => {
        localStorage.removeItem('authToken');
        setUser(null);
        window.location.replace('/administrador/login');
    }, []);

    return (
        <AuthContext.Provider value={{ user, isAuthenticated, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuth deve ser usado dentro de um AuthProvider');
    }
    return context;
};
