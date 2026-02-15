import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface SystemConfigurationDTO {
  id?: number;
  codigoSistema: string;
  licencia: string;
  nombreNegocio: string;
  direccion?: string;
  municipio?: string;
  provincia?: string;
  codPostal?: string;
  imagen?: string;
  web?: string;
  email?: string;
  telefono?: string;
  cuentaPagarId?: number;
  cuentaCobrarId?: number;
  caducidad: string;
  creadoPor?: string;
  fechaHoraCreado?: string;
  modificadoPor?: string;
  fechaHoraModificado?: string;
  // Propiedades adicionales para visualización
  cuentaPagarCodigo?: string;
  cuentaPagarDescripcion?: string;
  cuentaCobrarCodigo?: string;
  cuentaCobrarDescripcion?: string;
  estaVigente?: boolean;
  estaVencida?: boolean;
  proximoAVencer?: boolean;
  diasRestantes?: number;
  estadoLicencia?: string;
  tieneCuentasConfiguradas?: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class SystemConfigurationService {
  private readonly endpoint = 'SystemConfiguration';

  constructor(private apiService: ApiService) { }

  getAll(): Observable<SystemConfigurationDTO[]> {
    console.log('📊 Obteniendo todas las configuraciones del sistema...');
    return this.apiService.get<SystemConfigurationDTO[]>(this.endpoint);
  }

  getById(id: number): Observable<SystemConfigurationDTO> {
    console.log('📊 Obteniendo configuración del sistema:', id);
    return this.apiService.get<SystemConfigurationDTO>(`${this.endpoint}/${id}`);
  }

  create(dto: SystemConfigurationDTO): Observable<SystemConfigurationDTO> {
    console.log('📊 Creando configuración del sistema:', dto);
    return this.apiService.post<SystemConfigurationDTO>(this.endpoint, dto);
  }

  update(id: number, dto: SystemConfigurationDTO): Observable<SystemConfigurationDTO> {
    console.log('📊 Actualizando configuración del sistema:', id, dto);
    return this.apiService.put<SystemConfigurationDTO>(`${this.endpoint}/${id}`, dto);
  }
}
