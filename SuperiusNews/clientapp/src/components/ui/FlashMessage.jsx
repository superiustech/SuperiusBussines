import React, { useState, useEffect } from 'react';

const FlashMessage = ({ message, details, type = 'error', duration = 3000 }) => {
    const [visible, setVisible] = useState(false);
    const [fading, setFading] = useState(false);

    useEffect(() => {
        if (message) {
            setVisible(true);
            setFading(false);

            // Inicia o fade out pouco antes de desaparecer
            const fadeOutTimer = setTimeout(() => setFading(true), duration - 500);
            // Esconde o componente após o fade out
            const hideTimer = setTimeout(() => setVisible(false), duration);

            return () => {
                clearTimeout(fadeOutTimer);
                clearTimeout(hideTimer);
            };
        }
    }, [message, duration]);

    if (!visible) return null;

    const colors = {
        error: { bg: '#ffebee', border: '#f44336' },
        success: { bg: '#e8f5e9', border: '#4caf50' },
        warning: { bg: '#fff8e1', border: '#ffc107' },
        info: { bg: '#e3f2fd', border: '#2196f3' }
    };

    return (
        <div id="divMessage" style={{
            display: 'block',
            opacity: fading ? 0 : 1,
            transition: 'opacity 0.5s ease-in-out'
        }}>
            <div className="flash-message" style={{
                padding: '10px',
                background: colors[type].bg,
                borderLeft: `4px solid ${colors[type].border}`,
                marginBottom: '10px'
            }}>
                <strong>{message}</strong>
                {details && <div style={{ marginTop: '5px' }}>{details}</div>}
                <p style={{ margin: '5px 0 0 0', fontSize: '0.8em', color: '#666' }}>
                    Esta mensagem desaparecerá em {duration / 1000} segundos...
                </p>
            </div>
        </div>
    );
};

export default FlashMessage;