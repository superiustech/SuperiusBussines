import React from 'react';
import Sidebar from './Sidebar';
import MainContent from './MainContent';
import FlashMessage from '../ui/FlashMessage';

const Layout = ({ children }) => {
    return (
        <div className="d-flex">
            <Sidebar />
            <MainContent>
                <FlashMessage />
                {children}
            </MainContent>
        </div>
    );
};

export default Layout;