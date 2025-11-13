import React, { useState, useEffect } from 'react';

const FlashMessage = ({ message, details, type = 'error', duration = 3000 }) => {
    const [visible, setVisible] = useState(false);
    const [fading, setFading] = useState(false);

    useEffect(() => {
        if (message) {
            setVisible(true);
            setFading(false);

            const fadeOutTimer = setTimeout(() => setFading(true), duration - 500);
            const hideTimer = setTimeout(() => setVisible(false), duration);

            return () => {
                clearTimeout(fadeOutTimer);
                clearTimeout(hideTimer);
            };
        }
    }, [message, duration]);

    if (!visible) return null;

    const colors = {
        error: { bg: '#fdecea', border: '#f44336', icon: '' },
        success: { bg: '#edf7ed', border: '#4caf50', icon: '' },
        warning: { bg: '#fff8e1', border: '#ffc107', icon: '' },
        info: { bg: '#e3f2fd', border: '#2196f3', icon: '' }
    };

    return (
        <div
            id="divMessage"
            style={{
                display: 'flex',
                justifyContent: 'center',
                alignItems: 'center',
                position: 'fixed',
                top: '20px',
                left: 0,
                right: 0,
                zIndex: 9999,
                opacity: fading ? 0 : 1,
                transition: 'opacity 0.5s ease-in-out',
            }}
        >
            <div
                className="flash-message shadow"
                style={{
                    display: 'flex',
                    alignItems: 'flex-start',
                    gap: '10px',
                    padding: '15px 20px',
                    background: colors[type].bg,
                    borderLeft: `6px solid ${colors[type].border}`,
                    borderRadius: '8px',
                    boxShadow: '0 4px 12px rgba(0, 0, 0, 0.1)',
                    width: 'fit-content',
                    maxWidth: '90%',
                    minWidth: '300px',
                    fontFamily: 'Inter, Roboto, sans-serif',
                    animation: 'slideDown 0.4s ease',
                }}
            >
                <span style={{ fontSize: '1.5em', lineHeight: 1 }}>
                    {colors[type].icon}
                </span>
                <div style={{ flex: 1 }}>
                    <strong style={{ fontSize: '1rem' }}>{message}</strong>
                    {details && (
                        <div style={{ marginTop: '5px', fontSize: '0.9rem', color: '#333' }}>
                            {details}
                        </div>
                    )}
                    <p style={{ margin: '6px 0 0 0', fontSize: '0.75rem', color: '#777' }}>
                        Esta mensagem desaparecerá em {duration / 1000} segundos...
                    </p>
                </div>
            </div>

            <style>
                {`
                @keyframes slideDown {
                    from {
                        transform: translateY(-30px);
                        opacity: 0;
                    }
                    to {
                        transform: translateY(0);
                        opacity: 1;
                    }
                }
                `}
            </style>
        </div>
    );
};

export default FlashMessage;
