import React, { useState, useCallback, useMemo, useEffect } from 'react';
import { AgGridReact } from 'ag-grid-react';
import { useNavigate } from 'react-router-dom';
import 'ag-grid-community/styles/ag-grid.css';
import 'ag-grid-community/styles/ag-theme-alpine.css';
import funcionalidades from '../common/Funcionalidades';
import { useAuth } from '../common/AuthContext';

const RevendedorTable = ({ revendedores, loading, onEditarRevendedor, onExcluirRevendedor, onRefresh, onGerenciarUsuarios }) => {
    const [gridApi, setGridApi] = useState(null);
    const [selected, setSelected] = useState([]);
    const { validarFuncionalidade } = useAuth();

    const navigate = useNavigate();

    const columnDefs = useMemo(() => [
        { headerCheckboxSelection: true, checkboxSelection: true, headerName: "", field: "checkbox",width: 20, pinned: 'left', suppressMenu: true,suppressSorting: true, suppressFilter: true},
        { headerName: "#", field: "codigo", sortable: true, filter: true, resizable: true, width: 80 },
        { headerName: "Tipo", field: "tipoRevendedor", sortable: true, filter: true, resizable: true, width: 100 },
        { headerName: "Nome", field: "nome", sortable: true, filter: true, resizable: true, flex: 3, minWidth: 350 },
        { headerName: "Estoque", field: "estoque", sortable: true, filter: true, resizable: true, flex: 3, minWidth: 250 }
    ], []);

    const gridOptions = useMemo(() => ({
        defaultColDef: { sortable: true, filter: true, resizable: true, minWidth: 100, flex: 1 },
        rowSelection: 'multiple',
        pagination: true,
        paginationPageSize: 10
    }), []);

    useEffect(() => {
        if (gridApi && revendedores.length) {
            gridApi.sizeColumnsToFit();
        }
    }, [gridApi, revendedores]);

    return (
        <div>
            <>
            <div className="mt-4 mb-3 d-flex justify-content-between align-items-center">
                <div className="d-flex align-items-center">
                    {validarFuncionalidade(funcionalidades.EDITAR_REVENDEDORES) && (<button className="btn btn-primary me-2" onClick={() => navigate(`/administrador/cadastrar-revendedor`)}> Incluir </button>)}
                    {validarFuncionalidade(funcionalidades.EDITAR_REVENDEDORES) && (<button className="btn btn-primary me-2" disabled={selected.length !== 1} onClick={() => onEditarRevendedor(selected[0].codigo)}> Editar </button>)}
                    {validarFuncionalidade(funcionalidades.ASSOCIAR_REVENDEDOR_USUARIO) && (<button className="btn btn-primary me-2" disabled={selected.length !== 1} onClick={() => onGerenciarUsuarios(selected[0].codigo)}> Usuarios </button>)}
                    {validarFuncionalidade(funcionalidades.EXCLUIR_REVENDEDORES) && (<button className="btn btn-danger" disabled={selected.length === 0} onClick={() => onExcluirRevendedor(selected.map(item => item.codigo).join(","))}> Excluir </button>)}
                </div>
                <div>
                    <button onClick={onRefresh} className="btn btn-outline-secondary" disabled={loading}> {loading ? 'Atualizando...' : 'Atualizar Lista'}</button>
                </div>
            </div>

            <div className="ag-theme-alpine" style={{ height: 450, width: '100%' }}>
                <AgGridReact
                    key={revendedores.length}
                    rowData={revendedores || []}
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

export default RevendedorTable;
