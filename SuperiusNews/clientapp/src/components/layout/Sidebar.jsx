import React, { useState } from 'react';
import { NavLink } from 'react-router-dom';

const Sidebar = () => {
    const [collapsed, setCollapsed] = useState(false);
    const toggleSidebar = () => { setCollapsed(!collapsed);};

    const navItems = [
        { path: "/administrador/#", icon: "fa-home", text: " Dashboard" },
        { path: "/administrador/estoques", icon: "fa-chart-bar", text: " Estoque" },
        { path: "/administrador/cadastrar-estoque", icon: "fa-chart-bar", text: " Cadastrar Estoque" },
        { path: "/administrador/produtos", icon: "fa-box", text: " Produtos" },
        { path: "/administrador/cadastrar-produto", icon: "fa-box", text: " Cadastrar Produto" },
        { path: "/administrador/revendedores", icon: "fa-solid fa-truck-field", text: " Revendedores" },
        { path: "/administrador/cadastrar-revendedores", icon: "fa-solid fa-truck-field", text: " Cadastrar Revendedores" },
        { path: "/administrador/#", icon: "fa-gear", text: " Configurações" }
    ];

    return (
        <nav className={`sidebar h ${collapsed ? 'collapsed' : ''}`}
            style={{ background: 'linear-gradient(135deg, #1a1c2e 0%, #16181f 100%)' }}>

            <button className="toggle-btn m-12" style={{ marginRight: '40px' }} onClick={toggleSidebar}>
                <i className={`fas fa-chevron-${collapsed ? 'right' : 'left'}`}></i>
            </button>

            <div className="p-4">
                <h4 className="logo-text text-light fw-bold mb-0">Superius</h4>
                {!collapsed && <p className="text-light small">Administrador</p>}
            </div>

            <div className="nav flex-column">
                {navItems.map((item, index) => (
                    <NavLink key={index} to={item.path} className={({ isActive }) => `sidebar-link text-decoration-none text-light p-3 ${isActive ? 'active' : ''}` }>
                        <i className={`fas ${item.icon} me-3`}></i> {!collapsed && <span>{item.text}</span>}
                    </NavLink>
                ))}
            </div>

            {!collapsed && (
                <div className="profile-section mt-auto p-2">
                    <div className="d-flex align-items-center">
                        <img src="https://i.postimg.cc/qqL8WQVS/foto-perfil.png" style={{ height: '60px' }} className="rounded-circle" alt="Profile"/>
                        <div className="ms-3 profile-info"> <h6 className="text-white mb-0">Lucas Nogueira</h6>
                            <small className="text-light">Product Owner</small>
                        </div>
                    </div>
                </div>
            )}
        </nav>
    );
};

export default Sidebar;