import React from 'react';

const Loading = ({ message = 'Carregando...', details, type = 'info', show = true }) => {
    const colors = {
        error: { bg: '#ffebee', border: '#f44336' },
        success: { bg: '#e8f5e9', border: '#4caf50' },
        warning: { bg: '#fff8e1', border: '#ffc107' },
        info: { bg: '#e3f2fd', border: '#2196f3' }
    };

    if (!show) return null;

    return (
        <div id="divLoading" style={{ display: 'block' }}>
            <div className="loading-message" style={{
                padding: '10px',
                background: colors[type].bg,
                borderLeft: `4px solid ${colors[type].border}`,
                marginBottom: '10px'
            }}>
                <div style={{ display: 'flex', alignItems: 'center', gap: '10px' }}>
                    <div className="loading-spinner" style={{
                        border: `3px solid ${colors[type].bg}`,
                        borderTop: `3px solid ${colors[type].border}`,
                        borderRadius: '50%',
                        width: '20px',
                        height: '20px',
                        animation: 'spin 1s linear infinite'
                    }}></div>
                    <strong>{message}</strong>
                </div>

                {details && <div style={{ marginTop: '5px' }}>{details}</div>}
            </div>

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