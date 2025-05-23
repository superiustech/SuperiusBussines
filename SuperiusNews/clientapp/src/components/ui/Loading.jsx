import React, { useEffect, useState } from 'react';

const Loading = ({ message = 'Carregando...', details, type = 'info', show = true }) => {
    const colors = {
        error: { bg: '#ffebee', border: '#f44336' },
        success: { bg: '#e8f5e9', border: '#4caf50' },
        warning: { bg: '#fff8e1', border: '#ffc107' },
        info: { bg: '#e3f2fd', border: '#2196f3' }
    };

    const [internalShow, setInternalShow] = useState(false);
    const [startTime, setStartTime] = useState(0);

    useEffect(() => {
        if (show) {
            //setStartTime(Date.now());
            setInternalShow(true);
        } else {
            //const elapsed = Date.now() - startTime;
            //const minDuration = 800;

            //if (elapsed >= minDuration) {
                setInternalShow(false);
            //} else {
            //    setTimeout(() => setInternalShow(false), minDuration - elapsed);
            //}
        }
    }, [show]);

    if (!internalShow) return null;

    return (
        <div id="divLoading" style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', padding: '10px' }}>
            <div className="loading-spinner" style={{
                border: `3px solid ${colors[type].bg}`,
                borderTop: `3px solid ${colors[type].border}`,
                borderRadius: '50%',
                width: '30px',
                height: '30px',
                animation: 'spin 1s linear infinite'
            }}></div>

            <style>{`
                @keyframes spin {
                    0% { transform: rotate(0deg); }
                    100% { transform: rotate(360deg); }
                }
            `}</style>
        </div>
    );
};

export default Loading;
