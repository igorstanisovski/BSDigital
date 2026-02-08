export interface DepthPoint { 
    price: number; 
    cumulative: number; 
}

export interface DepthSnapshot { 
    bids: DepthPoint[]; 
    asks: DepthPoint[]; 
    timestamp: string; 
}