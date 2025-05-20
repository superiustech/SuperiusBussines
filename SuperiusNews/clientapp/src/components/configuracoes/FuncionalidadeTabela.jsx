import React, { useState, useMemo, useEffect } from 'react';
import { AgGridReact } from 'ag-grid-react';
import { useNavigate } from 'react-router-dom';
import 'ag-grid-community/styles/ag-grid.css';
import 'ag-grid-community/styles/ag-theme-alpine.css';

const FuncionalidadeTabela = ({ funcionalidades, loading, onEditarFuncionalidade, onAtivarFuncionalidades, onInativarFuncionalidades, onRefresh }) => {
    const [gridApi, setGridApi] = useState(null);
    const [selected, setSelected] = useState([]);
    const navigate = useNavigate();

    const columnDefs = useMemo(() => [
        { headerCheckboxSelection: true, checkboxSelection: true, headerName: "", field: "checkbox",width: 20, pinned: 'left', suppressMenu: true,suppressSorting: true, suppressFilter: true},
        { headerName: "#", field: "codigoFuncionalidade", sortable: true, filter: true, resizable: true, width: 80 },
        { headerName: "Nome", field: "nomeFuncionalidade", sortable: true, filter: true, resizable: true, width: 100 },
        { headerName: "Descrição", field: "descricaoFuncionalidade", sortable: true, filter: true, resizable: true, flex: 3, minWidth: 350 },
        { headerName: "Status", field: "ativa", sortable: true, filter: true, resizable: true, flex: 3, minWidth: 250 }
    ], []);

    const gridOptions = useMemo(() => ({
        defaultColDef: { sortable: true, filter: true, resizable: true, minWidth: 100, flex: 1 },
        rowSelection: 'multiple',
        pagination: true,
        paginationPageSize: 10
    }), []);

    useEffect(() => {
        if (gridApi && funcionalidades.length) {
            gridApi.sizeColumnsToFit();
        }
    }, [gridApi, funcionalidades]);

    return (
        <div>
            <>
            <div className="mt-4 mb-3 d-flex justify-content-between align-items-center">
                <div className="d-flex align-items-center">
                    <button className="btn btn-primary me-2" disabled={selected.length !== 1} onClick={() => onEditarFuncionalidade(selected[0].codigoFuncionalidade)}> Editar </button>
                    <button className="btn btn-dark me-2" disabled={selected.length === 0} onClick={() => onAtivarFuncionalidades(selected.map(item => item.codigoFuncionalidade).join(","))}> Ativar </button>
                    <button className="btn btn-secondary me-2 btn-danger" disabled={selected.length === 0} onClick={() => onInativarFuncionalidades(selected.map(item => item.codigoFuncionalidade).join(","))}> Inativar </button>
                </div>
                <div>
                    <button onClick={onRefresh} className="btn btn-outline-secondary" disabled={loading}> {loading ? 'Atualizando...' : 'Atualizar Lista'}</button>
                </div>
            </div>

            <div className="ag-theme-alpine" style={{ height: 450, width: '100%' }}>
                <AgGridReact
                    key={funcionalidades.length}
                    rowData={funcionalidades || []}
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
                    headerHeight={56}/>
            </div>
            </>
        </div>
    );
};

export default FuncionalidadeTabela;
