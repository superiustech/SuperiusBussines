import { createContext, useContext, useState, useCallback, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

const decodeToken = (token) => {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(
        atob(base64).split('').map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2)).join('')
    );
    return JSON.parse(jsonPayload);
};
const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {

    const [user, setUser] = useState(null);

    const navigate = useNavigate();

    const validateTokenAndSetUser = useCallback(() => {
        const token = localStorage.getItem('authToken');
        if (!token) {
            setUser(null);
            return false;
        }

        try {
            const decodedToken = decodeToken(token);
            const currentTime = Date.now() / 1000;
            if (decodedToken.exp < currentTime) {
                logout();
                return false;
            }
            setUser({
                name: decodedToken.unique_name,
                funcionalidades: decodedToken.Funcionalidades || []
            });
            return true;
        } catch {
            setUser(null);
            return false;
        }
    }, []);

    const isAuthenticated = useCallback(() => {
        return user !== null;
    }, [user]);

    const validarFuncionalidade = useCallback((funcionalidade) => {
        const funcionalidadesString = user?.funcionalidades;
        if (typeof funcionalidadesString !== 'string' || funcionalidadesString.trim() === '') {
            return false;
        }
        const funcionalidadesArray = funcionalidadesString.split(',');
        return funcionalidadesArray.includes(String(funcionalidade));
    }, [user]);

    const login = useCallback((token) => {
        localStorage.setItem('authToken', token);
        validateTokenAndSetUser();
        navigate('/administrador/produtos'); 
    }, [navigate, validateTokenAndSetUser]);

    const logout = useCallback(() => {
        localStorage.removeItem('authToken');
        setUser(null);
        navigate('/administrador/login');
    }, [navigate]);

    useEffect(() => {
        validateTokenAndSetUser();
    }, [validateTokenAndSetUser]);

    return (
        <AuthContext.Provider value={{ user, isAuthenticated, login, logout, validarFuncionalidade }}>
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
