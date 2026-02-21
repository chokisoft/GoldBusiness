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
    return this.apiService.get<SystemConfigurationDTO[]>(this.endpoint);
  }

  getById(id: number): Observable<SystemConfigurationDTO> {
    return this.apiService.get<SystemConfigurationDTO>(`${this.endpoint}/${id}`);
  }

  create(dto: SystemConfigurationDTO): Observable<SystemConfigurationDTO> {
    return this.apiService.post<SystemConfigurationDTO>(this.endpoint, dto);
  }

  update(id: number, dto: SystemConfigurationDTO): Observable<SystemConfigurationDTO> {
    return this.apiService.put<SystemConfigurationDTO>(`${this.endpoint}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.apiService.delete<void>(`${this.endpoint}/${id}`);
  }

  /**
   * Sube el archivo de logo al servidor.
   * Devuelve el nombre del archivo guardado en disco.
   */
  uploadLogo(codigoSistema: string, file: File): Observable<{ fileName: string }> {
    const formData = new FormData();
    // ✅ Las claves deben coincidir exactamente con las propiedades de UploadLogoRequest
    formData.append('file', file);
    formData.append('codigoSistema', codigoSistema);
    return this.apiService.postFormData<{ fileName: string }>(
      `${this.endpoint}/upload-logo`, formData
    );
  }

  /**
   * Devuelve la URL completa del logo para usar en [src] de <img>.
   */
  getLogoUrl(fileName: string): string {
    return this.apiService.buildUrl(`${this.endpoint}/logo/${fileName}`);
  }
}
