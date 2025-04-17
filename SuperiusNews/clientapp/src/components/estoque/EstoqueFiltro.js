import React, { useState, useEffect } from 'react';

const EstoqueFiltro = ({ onFilterChange, onClearFilters }) => {
    const [nameFilter, setNameFilter] = useState('');
    const [descriptionFilter, setDescriptionFilter] = useState('');

    useEffect(() => {
        const timer = setTimeout(() => {
            onFilterChange({ sNmEstoque: nameFilter, sDsEstoque: descriptionFilter});
        }, 500);
        return () => clearTimeout(timer);
    }, [nameFilter, descriptionFilter, onFilterChange]);

    const handleClear = () => {
        setNameFilter('');
        setDescriptionFilter('');
        onClearFilters();
    };

    return (
        <div className="row mb-3">
            <div className="col-md-4">
                <label htmlFor="sNmEstoque" className="form-label">Filtrar por Nome</label>
                <input
                    type="text"
                    id="sNmEstoque"
                    className="form-control"
                    placeholder="Digite o nome..."
                    value={nameFilter}
                    onChange={(e) => setNameFilter(e.target.value)}
                />
            </div>
            <div className="col-md-4">
                <label htmlFor="sDsEstoque" className="form-label">Filtrar por Descrição</label>
                <input
                    type="text"
                    id="sDsEstoque"
                    className="form-control"
                    placeholder="Digite a descrição..."
                    value={descriptionFilter}
                    onChange={(e) => setDescriptionFilter(e.target.value)}
                />
            </div>
            <div className="col-md-4 d-flex align-items-end">
                <button onClick={handleClear} className="btn btn-secondary">Limpar Filtros</button>
            </div>
        </div>
    );
};

export default EstoqueFiltro;