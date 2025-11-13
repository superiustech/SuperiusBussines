import React, { useState } from 'react';
import { NavLink } from 'react-router-dom';
import { useAuth } from '../common/AuthContext';
import funcionalidades from '../common/Funcionalidades';
import 'bootstrap/dist/css/bootstrap.min.css';

const Sidebar = () => {
    const [collapsed, setCollapsed] = useState(false);
    const { logout, validarFuncionalidade } = useAuth();
    const toggleSidebar = () => { setCollapsed(!collapsed); };

    const navItems = [
        { path: "/administrador/dashboard", icon: "fa-home", text: "Dashboard", funcionalidade: funcionalidades.VISUALIZAR_DASHBOARD },
        { path: "/administrador/estoques", icon: "fa-chart-bar", text: "Estoque", funcionalidade: funcionalidades.VISUALIZAR_ESTOQUES },
        { path: "/administrador/produtos", icon: "fa-box", text: "Produtos", funcionalidade: funcionalidades.VISUALIZAR_PRODUTOS },
        { path: "/administrador/revendedores", icon: "fa-solid fa-truck-field", text: "Revendedores", funcionalidade: funcionalidades.VISUALIZAR_REVENDEDORES },
        { path: "/administrador/configuracoes", icon: "fa-solid fa-gear", text: "Configurações", funcionalidade: funcionalidades.VISUALIZAR_CONFIGURACOES },
    ];

    // Links rápidos extras com funcionalidade
    const quickLinks = [
        { path: "/administrador/cadastrar-produto", icon: "fa-plus", text: "Novo Produto", funcionalidade: funcionalidades.EDITAR_PRODUTOS },
        { path: "/administrador/cadastrar-estoque", icon: "fa-plus-square", text: "Novo Estoque", funcionalidade: funcionalidades.EDITAR_ESTOQUES },
        { path: "/administrador/cadastrar-revendedor", icon: "fa-user-plus", text: "Novo Revendedor", funcionalidade: funcionalidades.EDITAR_ESTOQUES },
        { path: "/administrador/usuarios", icon: "fa-user", text: "Usuários", funcionalidade: funcionalidades.VISUALIZAR_USUARIOS },
        { path: "/administrador/funcionalidades", icon: "fa-cogs", text: "Funcionalidades", funcionalidade: funcionalidades.VISUALIZAR_FUNCIONALIDADES },
        { path: "/administrador/permissoes", icon: "fa-key", text: "Permissões", funcionalidade: funcionalidades.VISUALIZAR_PERMISSOES },
    ];

    return (
        <nav
            className={`sidebar h d-flex flex-column ${collapsed ? 'collapsed' : ''}`}
            style={{
                background: 'linear-gradient(135deg, var(--bs-primary) 0%, #16181f 100%)',
                padding: '0'
            }}
        >
            {/* Toggle + Logo */}
            <div className="p-4">
                <button className="toggle-btn m-12" style={{ marginRight: '40px' }} onClick={toggleSidebar}>
                    <i className={`fas fa-chevron-${collapsed ? 'right' : 'left'}`}></i>
                </button>
                <h4 className="text-light fw-bold mb-0">Superius</h4>
                {!collapsed && <p className="text-light small">Administrador</p>}
            </div>

            {/* Conteúdo principal + links rápidos */}
            <div className="flex-grow-1 d-flex flex-column justify-content-start">
                <div className="nav flex-column">
                    {navItems.map((item, index) =>
                        validarFuncionalidade(item.funcionalidade) && (
                            <NavLink
                                key={index}
                                to={item.path}
                                className={({ isActive }) =>
                                    `sidebar-link text-decoration-none text-light p-3 ${isActive && item.path !== "#" ? 'active' : ''}`
                                }
                            >
                                <i className={`fas ${item.icon} me-3`}></i>
                                {!collapsed && <span>{item.text}</span>}
                            </NavLink>
                        )
                    )}

                    {!collapsed && <hr
                        className="mb-2"
                        style={{
                            border: 'none',
                            height: '2px',
                            width: '80%',
                            backgroundColor: 'rgba(255,255,255,0.3)',
                            margin: '1rem auto',
                            borderRadius: '2px'
                        }}
                    />}

                    {quickLinks.map((item, index) =>
                        validarFuncionalidade(item.funcionalidade) && (
                            <NavLink
                                key={`quick-${index}`}
                                to={item.path}
                                className="sidebar-link text-decoration-none text-light p-2 ps-4"
                            >
                                <i className={`fas ${item.icon} me-2`}></i>
                                {!collapsed && <span>{item.text}</span>}
                            </NavLink>
                        )
                    )}
                </div>
                {!collapsed && <hr
                    className="mb-2"
                    style={{
                        border: 'none',
                        height: '2px',
                        width: '80%',
                        backgroundColor: 'rgba(255,255,255,0.3)',
                        margin: '1rem auto',
                        borderRadius: '2px'
                    }}
                />}

            </div>



            {/* Rodapé fixo */}
            {!collapsed && (
                <div
                    className="sidebar-footer p-2 text-light mb-4"
                    style={{
                        paddingLeft: '0',
                    }}
                >
                    <div
                        style={{
                            borderLeft: '4px solid #ffffff33',
                            paddingLeft: '10px',
                            marginLeft: '15px',
                            display: 'flex',
                            flexDirection: 'column'
                        }}
                    >
                        <div className="d-flex align-items-center mb-2">
                            <div>
                                <i className="fas fa-circle text-success me-2" style={{ fontSize: '0.7rem' }}></i>
                                <small>Sistema Online</small><br />
                                <i className="fas fa-calendar text-secondary me-2" style={{ fontSize: '0.7rem' }}></i>
                                <small>
                                    {new Date().toLocaleDateString('pt-BR', {
                                        weekday: 'long',
                                        day: '2-digit',
                                        month: 'long',
                                        year: 'numeric'
                                    })}
                                </small>
                            </div>
                        </div>

                        <NavLink
                            to="#"
                            onClick={() => logout()}
                            className="btn btn-outline-light btn-sm d-flex align-items-center justify-content-center mt-2"
                            style={{
                                borderRadius: '8px',
                                padding: '0.4rem 0.8rem',
                                fontSize: '0.9rem',
                                gap: '6px',
                                transition: 'all 0.2s'
                            }}
                        >
                            <i className="fas fa-sign-out-alt"></i>
                            <span>Sair</span>
                        </NavLink>
                    </div>
                </div>
            )}
        </nav>

    );
};

export default Sidebar;
