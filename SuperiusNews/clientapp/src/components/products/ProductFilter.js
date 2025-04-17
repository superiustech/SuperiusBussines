import React, { useState, useEffect } from 'react';
//import './ProductFilter.css';

const ProductFilter = ({ onFilterChange, onClearFilters }) => {
    const [nameFilter, setNameFilter] = useState('');
    const [descriptionFilter, setDescriptionFilter] = useState('');

    useEffect(() => {
        const timer = setTimeout(() => {
            onFilterChange({
                sNmProduto: nameFilter,
                sDsProduto: descriptionFilter
            });
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
                <label htmlFor="sNmProduto" className="form-label">Filtrar por Nome</label>
                <input
                    type="text"
                    id="sNmProduto"
                    className="form-control"
                    placeholder="Digite o nome..."
                    value={nameFilter}
                    onChange={(e) => setNameFilter(e.target.value)}
                />
            </div>
            <div className="col-md-4">
                <label htmlFor="sDsProduto" className="form-label">Filtrar por Descrição</label>
                <input
                    type="text"
                    id="sDsProduto"
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

export default ProductFilter;