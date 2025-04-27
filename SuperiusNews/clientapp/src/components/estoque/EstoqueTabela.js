import React, { useState, useMemo, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { AgGridReact } from 'ag-grid-react';
import 'ag-grid-community/styles/ag-grid.css';
import 'ag-grid-community/styles/ag-theme-alpine.css';

const EstoqueTabela = ({ estoques, loading , onEstoqueClick, onDeleteClick, onRefresh}) => {
    const [gridApi, setGridApi] = useState(null);
    const [selected, setSelected] = useState([]);
    const navigate = useNavigate();

    const columnDefs = useMemo(() => [
        { headerCheckboxSelection: true, checkboxSelection: true, headerName: "", field: "checkbox", width: 20, pinned: 'left', suppressMenu: true, suppressSorting: true, suppressFilter: true },
        { headerName: "#", field: "codigo", sortable: true, filter: true, resizable: true, width: 80 },
        { headerName: "C. Identificação", field: "codigoIdentificacao", sortable: true, filter: true, resizable: true, width: 100 },
        { headerName: "Nome", field: "nome", sortable: true, filter: true, resizable: true, flex: 3, minWidth: 350 },
        { headerName: "Descricao", field: "descricao", sortable: true, filter: true, resizable: true, flex: 3, minWidth: 350 }
    ], []);

    const gridOptions = useMemo(() => ({
        defaultColDef: { sortable: true, filter: true, resizable: true, minWidth: 100, flex: 1 },
        rowSelection: 'multiple',
        pagination: true,
        paginationPageSize: 10
    }), []);

    useEffect(() => {
        if (gridApi && estoques.length) {
            gridApi.sizeColumnsToFit();
        }
    }, [gridApi, estoques]);

    return (
        <div>
            <>
                <div className="mt-8 mb-3 d-flex justify-content-between align-items-center">
                    <div className="d-flex align-items-center">
                        <button className="btn btn-primary me-2" onClick={() => navigate(`/administrador/cadastrar-estoque`)}> Incluir </button>
                        <button className="btn btn-secondary me-2" disabled={selected.length !== 1} onClick={() => onEstoqueClick(selected[0].codigo)}> Editar </button>
                        <button className="btn btn-danger" disabled={selected.length === 0} onClick={() => onDeleteClick(selected.map(item => item.codigo).join(","))}> Excluir </button>
                    </div>
                    <div>
                        <button onClick={onRefresh} className="btn btn-outline-secondary" disabled={loading}> {loading ? 'Atualizando...' : 'Atualizar Lista'}</button>
                    </div>
                </div>

                <div className="ag-theme-alpine" style={{ height: 450, width: '100%' }}>
                    <AgGridReact
                        key={estoques.length}
                        rowData={estoques || []}
                        columnDefs={columnDefs}
                        onGridReady={(params) => setGridApi(params.api)}
                        gridOptions={gridOptions}
                        suppressReactUi={true}
                        animateRows={true}
                        onSelectionChanged={(event) => {
                            const selectedNodes = event.api.getSelectedNodes();
                            const selectedData = selectedNodes.map(node => node.data);
                            setSelected(selectedData);
                        }}
                        rowHeight={48}
                        headerHeight={56} />
                </div>
            </>
        </div>
    );
};

export default EstoqueTabela;