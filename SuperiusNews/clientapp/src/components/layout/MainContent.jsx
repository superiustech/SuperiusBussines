import React from 'react';

const MainContent = ({ children }) => {
    return (
        <div className="main-content">
            <main role="main" className="pb-3">
                {children}
            </main>
        </div>
    );
};

export default MainContent;