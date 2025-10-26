import {
  useContext,
  createContext,
  type ReactElement,
  useState,
  useEffect,
} from "react";
import { AccountService } from "../api/services/account-service";
import { useNavigate } from "react-router-dom";
import { jwtDecode } from "jwt-decode";
import { praseBool } from "./utils";
import { useQueryClient } from "@tanstack/react-query";

interface JwtPayload {
  role: string;
  mustChangePassword: string;
}

interface AuthContextType {
  jwtToken: string | null;
  role: string | null;
  mustChangePassword: boolean;
  login: (email: string, password: string) => Promise<void>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

const AuthProvider = ({ children }: { children: ReactElement }) => {
  const [jwtToken, setJwtToken] = useState<string | null>(
    localStorage.getItem("jwtToken")
  );
  const [role, setRole] = useState<string | null>(null);
  const [mustChangePassword, setMustChangePassword] = useState(false);
  const navigate = useNavigate();
  const queryClient = useQueryClient();

  const updateClaimsFromToken = (token: string | null) => {
    if (!token) {
      setRole(null);
      setMustChangePassword(false);
      return;
    }

    try {
      const decoded = jwtDecode<JwtPayload>(token);
      setRole(decoded.role);
      setMustChangePassword(praseBool(decoded.mustChangePassword));
    } catch {
      setRole(null);
      setMustChangePassword(false);
    }
  };

  useEffect(() => {
    updateClaimsFromToken(jwtToken);
  }, [jwtToken]);

  const login = async (email: string, password: string) => {
    const response = await AccountService.login({
      email: email,
      password: password,
    });

    localStorage.setItem("jwtToken", response.jwtToken);
    localStorage.setItem("refreshToken", response.refreshToken);

    setJwtToken(response.jwtToken);
    updateClaimsFromToken(response.jwtToken);

    navigate("/");
  };

  const logout = () => {
    localStorage.removeItem("jwtToken");
    localStorage.removeItem("refreshToken");

    setJwtToken(null);

    navigate("/login");

    queryClient.clear();
  };

  useEffect(() => {
    const handleStorageChange = () => {
      const token = localStorage.getItem("jwtToken");
      setJwtToken(token);
      updateClaimsFromToken(token);
    };

    window.addEventListener("storage", handleStorageChange);
    return () => window.removeEventListener("storage", handleStorageChange);
  });

  return (
    <AuthContext.Provider
      value={{ jwtToken, role, mustChangePassword, login, logout }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export default AuthProvider;

export const useAuth = () => {
  return useContext(AuthContext);
};
